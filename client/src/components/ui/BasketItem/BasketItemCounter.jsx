import api from '../../../services/AxiosService';
import styles from './BasketItem.module.css';

const BasketItemCounter = ({ basketItem, changeBasketItemCountProduct }) => {
  const decrement = () => {
    api
      .put(
        `api/customers/baskets/${basketItem.id}?productCount=${
          basketItem.productCount - 1
        }`
      )
      .then(
        changeBasketItemCountProduct(basketItem.id, basketItem.productCount - 1)
      );
  };

  const increment = () => {
    api
      .put(
        `api/customers/baskets/${basketItem.id}?productCount=${
          basketItem.productCount + 1
        }`
      )
      .then(
        changeBasketItemCountProduct(basketItem.id, basketItem.productCount + 1)
      );
  };

  return (
    <div style={{ display: 'flex', alignItems: 'center' }}>
      {basketItem.productCount == 1 ? (
        <button disabled className={styles.button}>
          -
        </button>
      ) : (
        <button onClick={decrement} className={styles.button}>
          -
        </button>
      )}
      <div style={{ padding: '0 10px' }}>{basketItem.productCount}</div>
      <button onClick={increment} className={styles.button}>
        +
      </button>
    </div>
  );
};

export default BasketItemCounter;
