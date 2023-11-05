import { useEffect, useState } from 'react';
import { Link, useParams } from 'react-router-dom';
import api from '../../../services/AxiosService';
import Header from './../../ui/Header/Header';
import ProductSlider from './../../ui/ProductSlider/ProductSlider';
import styles from './ProductDetail.module.css';
import Comments from '../../ui/Comments/Comments';
import { addBasketItem } from '../../../services/BasketService';
import { useContext } from 'react';
import { ApplicationContext } from '../../../providers/ApplicationProvider';

const ProductDetail = () => {
  const { id } = useParams();
  const [product, setProduct] = useState();
  const { basket, setBasket } = useContext(ApplicationContext);

  useEffect(() => {
    api.get(`/api/products/${id}`).then((response) => {
      setProduct(response.data);
      console.log(response.data);
    });
  }, []);

  useEffect(() => {
    if (!product?.category || !product?.manufacturer) {
      api
        .get(`/api/products/categories/${product?.categoryId}`)
        .then((response) =>
          setProduct({ ...product, category: response.data })
        );

      api
        .get(`/api/products/manufacturers/${product?.manufacturerId}`)
        .then((response) =>
          setProduct({ ...product, manufacturer: response.data })
        );
    }
  }, [product]);

  return (
    <div>
      <Header />
      <div className="container">
        <h2 className={styles.title}>{product?.productName}</h2>

        <div className={styles.body}>
          <div style={{ minWidth: '0px', width: '60%' }}>
            <ProductSlider
              imagesIds={product?.imagesIds}
              videosIds={product?.videosIds}
            />
          </div>

          <div className={styles.productInfo}>
            <div className={styles.productInfoPriceBlock}>
              <div className={`price ${styles.price}`}>
                {product?.unitPrice} Р
              </div>
              <div>
                {basket.find((bi) => bi.productId == id) ? (
                  <Link
                    className="button"
                    style={{
                      textAlign: 'center',
                      textDecoration: 'none',
                      fontSize: '15px',
                      padding: '15px 30px',
                      backgroundColor: '#10c44c',
                    }}
                    to="/basket"
                  >
                    Перейти в корзину
                  </Link>
                ) : (
                  <button
                    className="button"
                    style={{
                      fontSize: '15px',
                      padding: '15px 30px',
                    }}
                    onClick={async () => {
                      const response = await addBasketItem(+id, 1);
                      if (response.status == 200) {
                        setBasket([...basket, response.data]);
                      }
                    }}
                  >
                    Добавить в корзину
                  </button>
                )}
              </div>
            </div>

            <div className={styles.productDescription}>
              <div>
                В наличии{' '}
                <span className={styles.productDescriptionValue}>
                  {product?.quantityInStoke} шт
                </span>
              </div>
              <div style={{ marginBottom: '10px' }}>
                Описание:{' '}
                <span className={styles.productDescriptionValue}>
                  {product?.productDescription}
                </span>
              </div>

              <div>
                Категория:{' '}
                <span className={styles.productDescriptionValue}>
                  {product?.category}
                </span>
              </div>
              <div>
                Производитель:{' '}
                <span className={styles.productDescriptionValue}>
                  {product?.manufacturer}
                </span>
              </div>
            </div>
          </div>
        </div>

        <Comments productId={id} />
      </div>
    </div>
  );
};

export default ProductDetail;
