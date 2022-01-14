function downloadQR(data) {
    const xmlHttp = new XMLHttpRequest();
  
    xmlHttp.onload = () => {
      if (xmlHttp.status !== 200) {
        console.error(xmlHttp)
      }
  
      const link = document.createElement('a');
      link.download = 'qr.png';
      link.href = URL.createObjectURL(xmlHttp.response);
      link.click();
    }
  
    xmlHttp.onerror = (err) => {
      console.error(err);
      console.error(xmlHttp)
    }
  
    const url = `${CONFIG.qr.apiUrl}?size=${CONFIG.qr.size}&data=${data}`;
    xmlHttp.open('GET', url, true);
    xmlHttp.responseType = 'blob';
    xmlHttp.send(null);
  }
  