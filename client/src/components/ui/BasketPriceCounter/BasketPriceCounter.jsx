import { useEffect, useState } from 'react';
import styles from './BasketPriceCounter.module.css';

const BasketPriceCounter = ({ basket }) => {
  const [basketPrice, setBasketPrice] = useState(0);

  useEffect(() => {
    setBasketPrice(
      basket.reduce((acc, bi) => acc + bi.productUnitPrice * bi.productCount, 0)
    );
  }, [basket]);

  return (
    <div className={styles.counter}>
      <div>
        <button
          style={{
            textAlign: 'center',
            textDecoration: 'none',
            fontSize: '15px',
            padding: '15px 30px',
            backgroundColor: '#10c44c',
            marginBottom: '20px',
          }}
          className="button"
        >
          Перейти к оформлению
        </button>
      </div>
      <div>
        <h3 className={styles.basketPriceCounter__title}>Ваша корзина</h3>
        <div className={styles.basketPriceCounter__priceBlock}>
          <div>Товары ({basket.length})</div>
          <div>{basketPrice} Р</div>
        </div>
      </div>
    </div>
  );
};

export default BasketPriceCounter;
