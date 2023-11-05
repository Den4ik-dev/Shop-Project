import ReactInputMask from 'react-input-mask';

const InputPhone = ({ value, onChange }) => {
  return (
    <ReactInputMask
      className="input"
      mask="+7(999)-999-99-99"
      placeholder="+7(999)-999-99-99"
      value={value}
      onChange={(e) => onChange(e)}
    />
  );
};

export default InputPhone;
