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
        apiKey: 'AQHgtyJ6AwzOFL6nUA1bJBIu8gaEHSR1gEpKQn5QmOhoYYxKJnkoGHd4H+40dPe7plaz3ecK2qykaLKDZQgLgjwh/bnIBlKmXh0yvcMX0Sr8FN03QAFQ5FcPV0i0PuWj3hN+B87Ka7V06AtpdaXx4m32hIoX1SFJQfMzhEmQWhNZgVh5/b9TsbfOpiXnZ9MeLKQTlefplVzAAfmK5G6rRYs9uOi0NxbrC9gsJVtd2lAfyqCt6NoglIBAnBc1IVfvosqn5UJRbH3HUFV/U09K+5VN+aNe8SS9QLDGcncOAw07bmN944A2J4m6byGvVPt7ULExEayNgERuo6ZQGKDclj6Q+mKcImFPyV8xzYFglIWCwPzMf0RjxnEAqx8agftnKLRw6mYw9m2VoeJcKy7v9K2W4rxq+oHSNlqXKjUlKNddXxwiCxyFZdmaiY7f3ttYotEGjXpPx5cROkTvNrDok5Qa33li+G3gz2pUluAaZ+gGTI3T+XisCnmuuXRoJuV6Wwu6Y4kuPep+M4DJi/uyf79KcQ4S+6dPJ1Gk/BNym81DNAM0oG+OKIYIh6HGoFn2i+zYwvudw5lg4AlNqpgoHj1Wyt5IaD0aj6w2TabQFykJ8/2/rbjrQapMP7iukmd9Ra7kJy7Mp5yllvmVvu/a6s54HGOpP0Ea9sydr4QjVlpU/qXyb+PsYBASHvMYYmKSo2ob5hACLT/LEdZNJjGBkxH2mQWf+hedW0rRJIlyZmRi58UTFkBrG7f4o3RofhnA1sY3oG+R8j4qf9NCbJhgDVDrDAVMZaEHxiYuS3oHno74ag==',
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
