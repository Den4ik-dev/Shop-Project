import { useContext } from 'react';
import { ApplicationContext } from '../../../providers/ApplicationProvider';
import Header from '../../ui/Header/Header';
import BasketItem from '../../ui/BasketItem/BasketItem';
import styles from './Basket.module.css';
import BasketPriceCounter from '../../ui/BasketPriceCounter/BasketPriceCounter';
import api from '../../../services/AxiosService';

const Basket = () => {
  const { basket, setBasket } = useContext(ApplicationContext);

  const changeBasketItemCountProduct = (basketItemId, productCount) => {
    setBasket(
      basket.map((bi) => (bi.id != basketItemId ? bi : { ...bi, productCount }))
    );
  };

  const removeBasketItem = (basketItemId) => {
    api
      .delete(`api/customers/baskets/${basketItemId}`)
      .then(() => setBasket(basket.filter((bi) => bi.id != basketItemId)));
  };

  return (
    <div className="container" style={{ justifyContent: 'center' }}>
      <Header />
      <div className={styles.basket}>
        <div className={styles.basket__content}>
          {basket.map((basketItem) => (
            <BasketItem
              key={basketItem.id}
              basketItem={basketItem}
              changeBasketItemCountProduct={changeBasketItemCountProduct}
              removeBasketItem={removeBasketItem}
            />
          ))}
        </div>
        <div className={styles.basketCounterBlock}>
          <BasketPriceCounter basket={basket} />
        </div>
      </div>
    </div>
  );
};

export default Basket;
