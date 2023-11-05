import { useState } from 'react';
import styles from './RegisterForm.module.css';
import InputPhone from '../../ui/InputPhone';
import ErrorMessage from '../../ui/ErrorMessage';
import { registerCustomer } from '../../../services/CustomersService';
import { useNavigate } from 'react-router-dom';
import { Link } from 'react-router-dom';

const clearedUser = {
  firstName: '',
  lastName: '',
  address: '',
  city: '',
  birthDate: '',
  phone: '',
  email: '',
  password: '',
};

const RegisterForm = () => {
  const [registeredUser, setRegisteredUser] = useState(clearedUser);
  const [isRegistered, setIsRegistered] = useState(false);
  const [error, setError] = useState('');
  const nav = useNavigate();

  const onSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await registerCustomer(registeredUser);

      setError('');

      setIsRegistered(true);
      setRegisteredUser({ ...registeredUser, ...response.data });

      setTimeout(() => nav('/login'), 3000); // 3s
    } catch (e) {
      console.log(e);
      setError(e.response?.data?.message);
    }
  };

  return isRegistered ? (
    <div
      style={{
        position: 'absolute',
        transform: 'translate(-50%, -50%)',
        top: '50%',
        left: '50%',
        fontSize: '22px',
        fontWeight: '600',
      }}
    >
      Спасиба за регистрацию, {registeredUser.firstName}{' '}
      {registeredUser.lastName}! Ваша почта {registeredUser.email}
    </div>
  ) : (
    <form className={styles.form} onSubmit={onSubmit}>
      <div className={styles.form__content}>
        <div className={styles.form__title}>Регистрация</div>

        <div className={styles.row}>
          <div>
            <div className="input-title">Имя:</div>
            <input
              className="input"
              type="text"
              name="firstName"
              placeholder="Введите имя..."
              value={registeredUser.firstName}
              onChange={(e) =>
                setRegisteredUser({
                  ...registeredUser,
                  firstName: e.target.value,
                })
              }
            />
          </div>
          <div>
            <div className="input-title">Фамилия:</div>
            <input
              className="input"
              type="text"
              name="lastName"
              placeholder="Введите фамилию..."
              value={registeredUser.lastName}
              onChange={(e) =>
                setRegisteredUser({
                  ...registeredUser,
                  lastName: e.target.value,
                })
              }
            />
          </div>
        </div>
        <div className={styles.row}>
          <div>
            <div className="input-title">Адрес:</div>
            <input
              className="input"
              type="text"
              name="address"
              placeholder="Введите адрес..."
              value={registeredUser.address}
              onChange={(e) =>
                setRegisteredUser({
                  ...registeredUser,
                  address: e.target.value,
                })
              }
            />
          </div>
          <div>
            <div className="input-title">Город:</div>
            <input
              className="input"
              type="text"
              name="city"
              placeholder="Введите город..."
              value={registeredUser.city}
              onChange={(e) =>
                setRegisteredUser({ ...registeredUser, city: e.target.value })
              }
            />
          </div>
        </div>
        <div className={styles.row}>
          <div>
            <div className="input-title">Дата рождения:</div>
            <input
              type="date"
              name="birthDate"
              value={registeredUser.birthDate}
              onChange={(e) =>
                setRegisteredUser({
                  ...registeredUser,
                  birthDate: e.target.value,
                })
              }
            />
          </div>
          <div>
            <div className="input-title">Номер телефона:</div>
            <InputPhone
              value={registeredUser.phone}
              onChange={(e) => {
                setRegisteredUser({ ...registeredUser, phone: e.target.value });
              }}
            />
          </div>
        </div>
        <div className={styles.row}>
          <div>
            <div className="input-title">Почта:</div>
            <input
              className="input"
              type="email"
              name="email"
              placeholder="Введите почту..."
              value={registeredUser.email}
              onChange={(e) =>
                setRegisteredUser({ ...registeredUser, email: e.target.value })
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
              value={registeredUser.password}
              onChange={(e) =>
                setRegisteredUser({
                  ...registeredUser,
                  password: e.target.value,
                })
              }
            />
          </div>
        </div>
        <ErrorMessage error={error} />
        <Link to="/login">Войти в аккаунт</Link>

        <div style={{ marginTop: '30px' }}>
          <button className="button" type="submit">
            Зарегестрироваться
          </button>
        </div>
      </div>
    </form>
  );
};

export default RegisterForm;
