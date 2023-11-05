import { useEffect, useState } from 'react';
import api from '../../../services/AxiosService';

const ProductVideoSlide = ({ videoId }) => {
  const [videoPath, setVideoPath] = useState();

  useEffect(() => {
    api
      .get(`/api/products/videos/${videoId}`)
      .then((response) => setVideoPath(response.data))
      .catch((e) => console.log(e));
  }, []);

  return (
    <div>
      <video src={videoPath}></video>
    </div>
  );
};

export default ProductVideoSlide;
