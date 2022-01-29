const DEFAULT_SYMBOLOGIES = [
    ScanditSDK.Barcode.Symbology.CODE128,
    ScanditSDK.Barcode.Symbology.CODE39,
    ScanditSDK.Barcode.Symbology.DATA_MATRIX,
    ScanditSDK.Barcode.Symbology.QR,
    ScanditSDK.Barcode.Symbology.UPCA,
    ScanditSDK.Barcode.Symbology.UPCE,
    ScanditSDK.Barcode.Symbology.EAN13,
];

const CONFIG = {
    qr: {
        apiUrl: 'https://api.qrserver.com/v1/create-qr-code/',
        size: '500x500',
    },
    ui: {
        mainPageTitle: 'Label Demo',
        settingsPageTitle: 'Settings',
        maxInputs: 5,
    },
    sdk: {
        apiKey: 'AblhyBSdNQSSOiIJtAi/f4ICIhxXDfcj7GmAo5hZaOymfba/YlMqubZZliJ0BJUHGHemVRg/67vaX9cTuk6onqFdMdnDI1mU3FVNPlArbqkneGNOPCZp9Flj8um+VBfkJ07NmPhAKBv8WfDj/31ezZxNcs8aT/68Fk3TXDtE8GawfPmbdWDfz+MxUwlbP0vbjwL+Czs867Vv4qFcrEAatgstZCLcNKKUfSteQ9JsbfPimSkiu+8bA8aqos36HCc1SMCl0xYeW7og6hIaf4d4SHOuiJQjiCo9eBDZbZ+WJn3ZJCDByrVfDsWmTw4Z2ufEZ2ouDLcLFR3VIyqT2inXkVdY0kgnykJuOgWvnCW5rDb4ELh3fgFdPm5j63qo8M2Q2jipN8TfQVi1rRMWMXI9VDA4rIaV87RqIE9oGvAihd0VcimtI+qF6dgt3bzjl9ODe/eBf1SpJ3nkxJMx+AeRUxYTDfLwhJ4CBrVAwKv5YVvB0jMOqnNWPqlxmqbuIs2AO3Fb3xHmBK+gdGulXp+VtkF/TMjShukbJdFvAUc7NxR/loWnrLWVL/FOa6dwcI9IvXxPaiyOBodCVF87lWcCF6w0znT1yMmrhVYkFdhZUDedSnTIwPvIb4HgvyDnKZtV0M69tWTBvKOQgy3CDuWTP8S8nRqqmEJLHYLDjwulyvaGaKjNgcvdnLzc1ZSIDUBOwk2IBSqQzH72yBKCw+cy7/3KcTxA5xsa7dfe516qc7Kq1PbMENUBXM1D0RVVZEhdf2+CxvyaSiQgj1WfzVMoew9ZScSti/u0nXKDq+LKZVw6ka7tHyUW8EE3baC7SXrGpMl07HfhZdsMKqkDT0MYI/dtUW/wsg2OwwIgWAB/yFsmWaSC/ibo7A==',
        engineLocation: 'https://cdn.jsdelivr.net/npm/scandit-sdk@5.x/build',
    },
    barcodePicker: {
        playSoundOnScan: true,
        vibrateOnScan: true,
        accessCamera: false,
        visible: false,
        guiStyle: 'none',
        enableCameraSwitcher: true,
        videoFit: ScanditSDK.BarcodePicker.ObjectFit.COVER,
        cameraSettings: {
            resolutionPreference: ScanditSDK.CameraSettings.ResolutionPreference.FULL_HD
        }
    },
    scanSettings: {
        enabledSymbologies: DEFAULT_SYMBOLOGIES,
        codeDuplicateFilter: -1,
        maxNumberOfCodesPerFrame: 3,
    },
    activeSymbolCountMin: 5,
    activeSymbolCountMax: 35,
}

const DEFAULT_FIELD_CONFIG = { s: 'DEFAULT' };
const DEFAULT_COUNT = 1;


const ALLOWED_SYMBOLOGIES = Object.keys(ScanditSDK.Barcode.Symbology)
    .map(symbologyKey => ({
        key: symbologyKey,
        value: ScanditSDK.Barcode.Symbology[symbologyKey],
        label: getSymbologyLabel(ScanditSDK.Barcode.Symbology[symbologyKey]),
    }))
    .filter(symbology => typeof (symbology.value) === 'string');

const PICKER_SYMBOLOGIES = [{ key: 'DEFAULT', value: 'DEFAULT', label: 'Default' }, ...ALLOWED_SYMBOLOGIES];


function getSymbologyLabel(symbology) {
    const symbologyName = typeof (symbology) === 'string' ? symbology : '';

    return ScanditSDK.Barcode.Symbology.toHumanizedName(symbologyName);
}
