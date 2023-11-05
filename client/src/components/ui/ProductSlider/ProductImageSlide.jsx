import { useEffect, useState } from 'react';
import api from '../../../services/AxiosService';

const ProductImageSlide = ({ imageId }) => {
  const [imagePath, setImagePath] = useState();

  useEffect(() => {
    api
      .get(`/api/products/images/${imageId}`)
      .then((response) => setImagePath(response.data))
      .catch((e) => console.log(e));
  }, []);

  return (
    <div>
      <img src={imagePath} alt="product image" />
    </div>
  );
};

export default ProductImageSlide;
