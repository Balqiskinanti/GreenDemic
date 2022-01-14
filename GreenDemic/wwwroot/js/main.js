let fieldsSettings = Array(CONFIG.ui.maxInputs).fill(0).map(() => ({ ...DEFAULT_FIELD_CONFIG }));
let fieldsCount = DEFAULT_COUNT;
let currentFieldsSettings = fieldsSettings.slice(0, fieldsCount);

let activeTab = 0;
let counter = 0;
let results = [];
let takenIndexes = [];
let settingsVisited = false;

// scan handlers

const onError = err => {
    appComponents.dimmerLoader.classList.remove('active');
    alert(err);
    console.error(err);
}

const onCameraError = err => {
    appComponents.scanStartButton.classList.remove('loading');
    alert('Could not access camera: ' + err.name);
    console.error(err);
}

const onScanError = err => {
    alert(err);
    console.error(err);
}

const onPickerReady = () => {
    appComponents.scanStartButton.classList.remove('disabled');
    appComponents.dimmerLoader.classList.add('opacity-hidden');
    appComponents.cameraIcon.classList.remove('display-hidden');
    appComponents.cameraIcon.classList.remove('opacity-hidden');
}

const onScan = scanResult => {
    const qrCodeSettings = getSettingsFromQRCode(scanResult);

    if (qrCodeSettings) {
        applySettingsFromQRCode(qrCodeSettings);

        appComponents.barcodePicker.pauseScanning();
        appComponents.barcodePicker.setVisible(false);
        return;
    }

    scanResult.barcodes.forEach(barcode => {
        if (results.includes(barcode.data)) return;
        if (isQRSettingsOnly()) return;

        const index = getProperInputIndex(barcode);

        if (index === -1) return;

        results.push(barcode.data);
        takenIndexes.push(index);
        appComponents.barcodeInputs[index].value = barcode.data;
        localStorage.setItem("code", appComponents.barcodeInputs[index].value);
        appComponents.barcodeInputTicks[index].classList.remove('opacity-hidden');

        counter++;

        if (counter === 1) {
            appComponents.scanStartButton.classList.remove('disabled');
        }

        if (counter === fieldsCount) {
            appComponents.barcodePicker.pauseScanning();
            appComponents.barcodePicker.setVisible(false);
        }
    });
}

const onCameraAccess = () => {
    appComponents.barcodePicker.resumeScanning()
        .then(() => {
            appComponents.barcodePicker.clearSession();
            appComponents.scanStartButton.classList.remove('loading');
            appComponents.cameraIcon.classList.add('display-hidden');
            appComponents.barcodePicker.setVisible(true);
        })
}

const onStart = () => {
    for (const element of appComponents.barcodeInputs) {
        element.value = '';
    }

    for (const element of appComponents.barcodeInputTicks) {
        element.classList.add('opacity-hidden');
    }

    appComponents.barcodePickerContainer.classList.remove('partially-faded');
    appComponents.dimmerLoader.classList.remove('active');
    appComponents.scanStartButton.classList.add('disabled', 'loading');

    counter = 0;
    results = [];
    takenIndexes = [];

    appComponents.barcodePicker
        .accessCamera()
        .then(onCameraAccess)
        .catch(onCameraError);
}

const applySettingsFromQRCode = (qrSettings) => {
    goToSettings(qrSettings.length);
    onSliderChange(qrSettings.length);

    qrSettings.forEach((setting, index) => {
        appComponents.symbologiesSelects[index].value = setting.s;
        appComponents.regExpInputs[index].value = setting.r || '';

        if (setting.r !== null && setting.r !== undefined) {
            updateInputStyling(index);
        }

        onSymbologiesChange({ value: setting.s, index });
        onRegExpChange({ value: setting.r, index });
    });

    goToMainPage();
    onStart();
    openSettingsSnackbar();
    appComponents.inputsSlider.value = qrSettings.length;
}

const updateSettings = () => {
    const scanSettings = new ScanditSDK.ScanSettings({
        ...CONFIG.scanSettings,
        enabledSymbologies: [...getSymbologiesFromSettings(currentFieldsSettings), ScanditSDK.Barcode.Symbology.QR],
        // we are always adding QR to be able to load settings
    });

    [...scanSettings.symbologySettings].forEach(([key, settings]) => {
        settings.setActiveSymbolCountsRange(CONFIG.activeSymbolCountMin, CONFIG.activeSymbolCountMax);
    });

    appComponents.barcodePicker.applyScanSettings(scanSettings);
}

const getProperInputIndex = barcode => {
    const freeSlots = currentFieldsSettings.filter((_, index) => !takenIndexes.includes(index));

    const prio1 = freeSlots
        .filter(fullMatch(barcode.data, barcode.symbology))
        .sort(sortByRegExpLength);

    const prio2 = freeSlots
        .filter(fullMatch(barcode.data, barcode.symbology, true))
        .sort(sortByRegExpLength);

    const prio0 = [...prio1, ...prio2].sort(sortByRegExpLength);

    const prio3 = freeSlots.filter(symbologyOnly(barcode.symbology));
    const prio4 = freeSlots.filter(symbologyOnly(barcode.symbology, true));

    const combinedSlots = [...prio0, ...prio3, ...prio4];

    return combinedSlots[0] ? currentFieldsSettings.findIndex(setting => combinedSlots[0] === setting) : -1;
}

const matchRegExp = data => setting => setting.r && RegExp(setting.r).test(data)
const matchSymbology = (symbology, isDefault) => setting => {
    return (!isDefault && setting.s === symbology) ||
        (isDefault && setting.s === 'DEFAULT' && DEFAULT_SYMBOLOGIES.includes(symbology));
};

const symbologyOnly = (symbology, isDefault) => setting => !setting.r && matchSymbology(symbology, isDefault)(setting);
const fullMatch = (data, symbology, isDefault) => setting => matchRegExp(data)(setting) && matchSymbology(symbology, isDefault)(setting);
const sortByRegExpLength = (a, b) => b.r.length - a.r.length;

const isQRSettingsOnly = symbology => {
    const enabledSymbologies = getSymbologiesFromSettings(currentFieldsSettings);
    const qr = ScanditSDK.Barcode.Symbology.QR;

    // check if qr was only for loading settings
    if (symbology === qr && !enabledSymbologies.includes(qr)) return;
}

const getSymbologiesFromSettings = (settings) => {
    return settings
        .reduce((arr, { s }) => s === 'DEFAULT' ? [...arr, ...DEFAULT_SYMBOLOGIES] : [...arr, s], []);
}

ScanditSDK.configure(CONFIG.sdk.apiKey, { engineLocation: CONFIG.sdk.engineLocation })
    .then(() => ScanditSDK.BarcodePicker.create(appComponents.barcodePickerContainer, CONFIG.barcodePicker))
    .then(createdPicker => {
        const scanSettings = new ScanditSDK.ScanSettings(CONFIG.scanSettings);

        appComponents.barcodePicker = createdPicker
        appComponents.barcodePicker.applyScanSettings(scanSettings);

        appComponents.barcodePicker
            .on('scan', onScan)
            .on('scanError', onScanError)
            .on('ready', onPickerReady);

        appComponents.scanStartButton.addEventListener('click', onStart);
    })
    .catch(onError);


addInputs(fieldsCount);
initSnackbar();
// settings handling

const settingsButton = document.querySelector('#settings-button');
settingsButton.addEventListener('click', () => goToSettings());

const saveSettingsButton = document.querySelector('#save-settings-button');
saveSettingsButton.addEventListener('click', () => goToMainPage());

const downloadButton = document.querySelector('#download-button');
downloadButton.addEventListener('click', () => downloadQR(JSON.stringify(currentFieldsSettings)));

function goToSettings(count = fieldsCount) {
    transitionToSettings();
    appComponents.barcodePicker.setVisible(false);

    appComponents.barcodePicker.pauseScanning();

    if (settingsVisited) return;

    initInputsSlider(count);
    addTabs(count);
    initTabBar();
    addTabsContents(CONFIG.ui.maxInputs);
    setSymbologySelectValue(0);

    settingsVisited = true;
}

function goToMainPage() {
    transitionToMain();
    updateSettings();
    onPickerReady();
}

// material handlers

const initInputsSlider = (value) => {
    if (appComponents.inputsSlider) return;

    appComponents.inputsSlider = new mdc.slider.MDCSlider(document.querySelector('#inputs-slider'));
    appComponents.inputsSlider.listen('MDCSlider:change', () => onSliderChange(appComponents.inputsSlider.value));
    appComponents.inputsSlider.value = value;
}

const initTabBar = () => {
    const firstInit = !appComponents.tabBar;

    appComponents.tabBar = new mdc.tabBar.MDCTabBar(document.querySelector('.mdc-tab-bar'));

    if (!firstInit) return;

    appComponents.tabBar.listen('MDCTabBar:activated', event => changeTab(event.detail.index));
}

const changeTab = index => {
    if (activeTab === index) return;
    activeTab = index;

    appComponents.tabBar.activateTab(index);
    const tabContentElements = document.querySelectorAll('.tab-content');

    document.querySelector('.tab-content--active').classList.remove('tab-content--active');

    tabContentElements[index].classList.add('tab-content--active');

    setSymbologySelectValue(index);
}

const onSliderChange = count => {
    if (count === fieldsCount) return;

    const newTabIndex = Math.min(count - 1, activeTab);

    resetInputs();
    addInputs(count);

    removeAllTabs();
    addTabs(count)
    initTabBar();
    appComponents.tabBar.activateTab(newTabIndex);

    // MDCTabBar:activated wont run on index 0 when it is current index
    if (newTabIndex === 0) {
        changeTab(newTabIndex);
    }

    fieldsCount = count;
    currentFieldsSettings = fieldsSettings.slice(0, fieldsCount);
}

const onSettingChange = key => ({ index, value }) => {
    fieldsSettings[index][key] = value;
    currentFieldsSettings = fieldsSettings.slice(0, fieldsCount);
}

const onSymbologiesChange = onSettingChange('s');
const onRegExpChange = onSettingChange('r');


const setSymbologySelectValue = (index) => {
    if (!appComponents.symbologiesSelects[index]) return;

    appComponents.symbologiesSelects[index].value = appComponents.symbologiesSelects[index] ?
        appComponents.symbologiesSelects[index].value || PICKER_SYMBOLOGIES[0].value :
        appComponents.symbologiesSelects[index];
}
