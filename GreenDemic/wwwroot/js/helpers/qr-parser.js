const isValidQRBarcode = barcode => {
    return barcode.symbology === ScanditSDK.Barcode.Symbology.QR && barcode.data && typeof(barcode.data) === 'string';
  }
  
  const isValidParsedQRData = data => {
    return Array.isArray(data) && data.length > 0 && data.length <= CONFIG.ui.maxInputs;
  }
  
  const areValidSettings = data => {
    const result =  data.every(setting => {
      return (
        Object.keys(setting).length > 0 &&
        Object.keys(setting).length <= 2 &&
        !!setting.s &&
        Object.keys(setting).every(key => ['s', 'r'].includes(key))
      );
    });
  
    return result;
  }
  
  const getSettingsFromQRCode = (scanResults) => {
    const foundBarcode = scanResults.barcodes
      .find(barcode => {
        if (!isValidQRBarcode(barcode)) return false;
  
        try {
          const parsedJson = JSON.parse(barcode.data);
          return isValidParsedQRData(parsedJson) && areValidSettings(parsedJson);
        } catch (err) {
          return false;
        }
      });
  
    return foundBarcode && JSON.parse(foundBarcode.data)
  }