function downloadXml(data, fileName) {
    try {
        // Convert data to XML
        let xmlString = '<Numbers>\n';
        data.forEach(item => {
            xmlString += `  <Number>\n`;
            xmlString += `    <Value>${item.value}</Value>\n`;
            xmlString += `    <IsPrime>${item.isPrime}</IsPrime>\n`;
            xmlString += `  </Number>\n`;
        });
        xmlString += '</Numbers>';

        // Create a Blob and trigger download
        const blob = new Blob([xmlString], { type: 'application/xml' });
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = fileName;
        link.click();
        URL.revokeObjectURL(link.href);
    } catch (error) {
        console.error('Error in downloadXml:', error);
    }
}

function downloadBinary(base64Data, fileName) {
    try {
        // Convert base64 to binary
        const binaryString = atob(base64Data);
        const bytes = new Uint8Array(binaryString.length);
        
        for (let i = 0; i < binaryString.length; i++) {
            bytes[i] = binaryString.charCodeAt(i);
        }

        // Create and trigger download
        const blob = new Blob([bytes], { type: 'application/octet-stream' });
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        setTimeout(() => URL.revokeObjectURL(link.href), 100);
    } catch (error) {
        console.error('Download failed:', error);
    }
}
