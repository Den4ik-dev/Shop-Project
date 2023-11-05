import { Link, useNavigate } from 'react-router-dom';
import styles from './Header.module.css';
import { useContext, useState } from 'react';
import { ApplicationContext } from '../../../providers/ApplicationProvider';

const Header = ({ initInputValue = '' }) => {
  const { user } = useContext(ApplicationContext);
  const [input, setInput] = useState(initInputValue);
  const nav = useNavigate();

  return (
    <header className={styles.header}>
      <div className={styles.header__container}>
        <div className={styles.header__column}>
          <div>
            <Link to="/" className={styles.shopIcon}>
              shop
            </Link>
          </div>

          <div>
            <button className={`button ${styles.catalog}`}>Каталог</button>
          </div>

          <div className={styles.search}>
            <input
              value={input}
              onChange={(e) => setInput(e.target.value)}
              type="text"
              className="input"
            />
            <button
              className="button"
              onClick={() => nav(`/search?text=${input}`)}
            >
              <div>
                <img
                  src="./../../../../public/header/search-icon.svg"
                  alt="Поиск"
                />
              </div>
            </button>
          </div>
        </div>

        <div className={styles.header__column}>
          {user.isLogin ? (
            <Link to="/account">
              <div>аккаунт</div>
            </Link>
          ) : (
            <Link to="/login">
              <div>
                <div>
                  <img
                    src="./../../../../public/header/login-icon.svg"
                    alt="Войти"
                  />
                </div>
                <div>Войти</div>
              </div>
            </Link>
          )}
          <Link>
            <div>
              <div>
                <img
                  src="./../../../../public/header/orders-icon.svg"
                  alt="Заказы"
                />
              </div>
              <div>Заказы</div>
            </div>
          </Link>
          <Link to="/basket">
            <div>
              <div>
                <img
                  src="./../../../../public/header/header-basket-icon.svg"
                  alt="Корзина"
                />
              </div>
              <div>Корзина</div>
            </div>
          </Link>
        </div>
      </div>
    </header>
  );
};

export default Header;
