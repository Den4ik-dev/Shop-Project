import { useContext, useEffect } from 'react';
import Header from '../../ui/Header/Header';
import { ApplicationContext } from '../../../providers/ApplicationProvider';

const Account = () => {
  const { user } = useContext(ApplicationContext);

  return (
    <div style={{ marginTop: '75px' }}>
      <Header />

      <div>Имя: {user.firstName}</div>
      <div>Фамилия: {user.lastName}</div>
      <div>
        Дата рождения:{' '}
        {new Date(user.birthDate).toLocaleString('ru', {
          year: 'numeric',
          month: 'long',
          day: 'numeric',
          timezone: 'UTC',
        })}
      </div>
      <div>Номер телефона: {user.phone}</div>
      <div>Город: {user.city}</div>
      <div>Адрес проживания: {user.address}</div>
    </div>
  );
};

export default Account;
