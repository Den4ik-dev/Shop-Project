import { useState } from 'react';
import api from '../../../services/AxiosService';

const CommentsInput = ({ productId, addComment }) => {
  const [comment, setComment] = useState('');

  return (
    <div style={{ display: 'flex', marginBottom: '50px' }}>
      <input
        className="input"
        value={comment}
        onChange={(e) => setComment(e.target.value)}
        type="text"
        style={{ width: '100%' }}
      />
      <button
        className="button"
        onClick={() => {
          api
            .post(`/api/products/comments`, {
              content: comment,
              productId: productId,
            })
            .then((response) => {
              addComment(response.data);
              setComment('');
            });
        }}
        style={{ maxWidth: '250px' }}
      >
        Прокомментировать
      </button>
    </div>
  );
};

export default CommentsInput;
