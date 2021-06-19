import React from 'react';
import axios from 'axios';
import { useDispatch } from 'react-redux';
import { addRole } from '../redux/actions/user';

import { Form, Input, Button } from 'antd';

const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
  };

const tailLayout = {
wrapperCol: { offset: 8, span: 16 },
};

function Login() {
    const dispatch = useDispatch();

    const onSubmitHandler = e => {
        axios.post("https://localhost:5001/api/auth/login", { email: e.email, password: e.password }, { withCredentials: true })
            .then(res => {
                dispatch(addRole(res.data.role));
            })
            .catch(err => console.error(err))
    }

    return (
        <div style={{display: 'flex', justifyContent: 'center', marginTop: '120px'}}>
            <div>
                <Form
                    {...layout}
                    name="basic"
                    initialValues={{ remember: true }}
                    onFinish={onSubmitHandler}
                    //onFinishFailed={}
                    >
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

export default Login
