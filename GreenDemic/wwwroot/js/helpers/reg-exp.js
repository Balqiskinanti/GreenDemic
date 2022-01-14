const handleRegExpValidity = (isValid, index) => {
    const input = document.querySelector(`#regexp-field-${index}`);
  
    if (!input) return;
  
    if (!isValid) {
      document.querySelector(`#regexp-field-${index}`).classList.add('mdc-text-field--invalid');
    } else {
      document.querySelector(`#regexp-field-${index}`).classList.remove('mdc-text-field--invalid');
    }
  }
  
  const isRegExpValid = regExpStr => {
    if (!regExpStr) return true;
  
    try {
      new RegExp(regExpStr);
      return true;
    } catch (e) {
      return false;
    }
  }
  
  const onRegExpBlur = ({ index, value }) => {
    handleRegExpValidity(isRegExpValid(value), index);
  }