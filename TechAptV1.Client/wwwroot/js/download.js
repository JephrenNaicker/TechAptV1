﻿function downloadXml(data, fileName) {
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
        const binaryData = atob(base64Data);
        const bytes = new Uint8Array(binaryData.length);
        for (let i = 0; i < binaryData.length; i++) {
            bytes[i] = binaryData.charCodeAt(i);
        }

        // Create a Blob and trigger download
        const blob = new Blob([bytes], { type: 'application/octet-stream' });
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = fileName;
        link.click();
        URL.revokeObjectURL(link.href);
    } catch (error) {
        console.error('Error in downloadBinary:', error);
    }
}
