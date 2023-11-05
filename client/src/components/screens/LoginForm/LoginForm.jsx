import styles from './LoginForm.module.css';
import { useState } from 'react';
import ErrorMessage from '../../ui/ErrorMessage';
import { Link, useNavigate } from 'react-router-dom';
import { loginCustomer } from '../../../services/CustomersService';

const clearedUser = {
  email: '',
  password: '',
};

const LoginForm = () => {
  const [loginUser, setLoginUser] = useState(clearedUser);
  const [error, setError] = useState('');
  const nav = useNavigate();

  const onSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await loginCustomer(loginUser);

      setError('');

      localStorage.setItem('token', JSON.stringify(response.data));

      nav('/');
    } catch (e) {
      console.log(e);
      setError(e.response?.data?.message);
    }
  };

  return (
    <form className={styles.form} onSubmit={onSubmit}>
      <div className={styles.form__title}>Вход в аккаунт</div>

      <div>
        <div className="input-title">Почта:</div>
        <input
          className="input"
          type="email"
          name="email"
          placeholder="Введите почту..."
          value={loginUser.email}
          onChange={(e) =>
            setLoginUser({ ...loginUser, email: e.target.value })
          }
        />
      </div>
      <div>
        <div className="input-title">Пароль</div>
        <input
          className="input"
          type="password"
          name="password"
          placeholder="Введите пароль..."
          value={loginUser.password}
          onChange={(e) =>
            setLoginUser({ ...loginUser, password: e.target.value })
          }
        />
      </div>

      <ErrorMessage error={error} />

      <Link to="/reg">Зарегестрироваться</Link>

      <div style={{ marginTop: '30px' }}>
        <button className="button" type="submit">
          Войти
        </button>
      </div>
    </form>
  );
};

export default LoginForm;
