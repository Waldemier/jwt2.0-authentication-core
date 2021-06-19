import React from 'react';
import axios from 'axios';
import { useHistory } from 'react-router-dom';

import { Form, Input, Button } from 'antd';

const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
};

const tailLayout = {
    wrapperCol: { offset: 8, span: 16 },
};


function Register() {
    let history = useHistory();

    const onRegisterHandler = e => {
        axios.post("https://localhost:5001/api/auth/register", {
            name: e.name,
            email: e.email,
            password: e.password,
            confirm: e.confirm
        })
            .then(res => console.log(res.data.status, res.status))
            .catch(err => console.error(err))
        history.push("/login");
    }

    return (
        <div style={{display: 'flex', justifyContent: 'center', marginTop: '120px'}}>
            <div>
                <Form
                    {...layout}
                    name="basic"
                    initialValues={{ remember: true }}
                    onFinish={onRegisterHandler}
                    //onFinishFailed={}
                >
                    <Form.Item
                        label="Name"
                        name="name"
                        rules={[{ required: true, message: 'Please input your name' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        label="Email"
                        name="email"
                        rules={[{ required: true, message: 'Please input your email' }]}
                    >
                        <Input />
                    </Form.Item>
                    <Form.Item
                        label="Password"
                        name="password"
                        rules={[{ required: true, message: 'Please input your password' }]}
                    >
                        <Input.Password />
                    </Form.Item>

                    <Form.Item
                        label="Confirm"
                        name="confirm"
                        rules={[{ required: true, message: 'Please input your confirm' }]}
                    >
                        <Input.Password />
                    </Form.Item>

                    <Form.Item {...tailLayout}>
                        <Button type="primary" htmlType="submit">
                            Submit
                        </Button>
                    </Form.Item>
                </Form>
            </div>
        </div>
    )
}

export default Register;
