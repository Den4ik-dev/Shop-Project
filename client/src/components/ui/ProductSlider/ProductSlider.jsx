import { Swiper, SwiperSlide } from 'swiper/react';
import 'swiper/css';
import { memo, useState } from 'react';
import styles from './ProductSlider.module.css';
import { Navigation, Thumbs } from 'swiper/modules';
import ProductImageSlide from './ProductImageSlide';
import ProductVideoSlide from './ProductVideoSlide';

const ProductSlider = ({ imagesIds, videosIds }) => {
  const [activeThumb, setActiveThumb] = useState();

  return (
    <div className={styles.slider}>
      <Swiper
        spaceBetween={10}
        modules={[Navigation, Thumbs]}
        thumbs={{ swiper: activeThumb }}
        className={styles.bigSlider}
      >
        {imagesIds?.map((imageId) => (
          <SwiperSlide key={imageId}>
            <ProductImageSlide imageId={imageId} />
          </SwiperSlide>
        ))}
        {videosIds?.map((videoId) => (
          <SwiperSlide key={videoId}>
            <ProductVideoSlide videoId={videoId} />
          </SwiperSlide>
        ))}
      </Swiper>

      <Swiper
        spaceBetween={10}
        slidesPerView={4}
        onSwiper={setActiveThumb}
        modules={[Navigation, Thumbs]}
        className={styles.miniSlider}
      >
        {imagesIds?.map((imageId) => (
          <SwiperSlide key={imageId}>
            <ProductImageSlide imageId={imageId} />
          </SwiperSlide>
        ))}
        {videosIds?.map((videoId) => (
          <SwiperSlide key={videoId}>
            <ProductVideoSlide videoId={videoId} />
          </SwiperSlide>
        ))}
      </Swiper>
    </div>
  );
};

export default memo(ProductSlider);
