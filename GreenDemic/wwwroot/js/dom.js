

/**
 * Navigation
 */

 function transitionToSettings() {
    const mainPage = document.querySelector('#main-page');
    const settingsPage = document.querySelector('#settings-page');
  
    mainPage.classList.add('display-hidden');
    settingsPage.classList.remove('display-hidden');
  
    document.title = CONFIG.ui.settingsPageTitle;
  }
  
  
  function transitionToMain() {
    const mainPage = document.querySelector('#main-page');
    const settingsPage = document.querySelector('#settings-page');
  
    mainPage.classList.remove('display-hidden');
    settingsPage.classList.add('display-hidden');
  
    document.title = CONFIG.ui.mainPageTitle;
  }
  
  /**
   * General helpers
   */
  
  function createElementFromHTML(htmlString) {
    const div = document.createElement('div');
    div.innerHTML = htmlString.trim();
  
    return div.firstChild;
  }
  
  function createElementsFromHTML(htmlString) {
    const div = document.createElement('div');
    div.innerHTML = htmlString.trim();
  
    return div.childNodes;
  }
  
  
  /**
   * Snackbar manipulation
   */
  
  function initSnackbar() {
    const snackbarElement = document.getElementById('settings-snackbar');
  
    appComponents.settingsSnackbar = new mdc.snackbar.MDCSnackbar(snackbarElement);
    appComponents.settingsSnackbar.timeoutMs = 4000;
  }
  
  function openSettingsSnackbar() {
    appComponents.settingsSnackbar.open();
  }
  
  /**
   * Inputs manipulation
   */
  
  function addInputs(count) {
    const array = Array(count).fill(0);
  
    addInputNodes(count);
  
    appComponents.barcodeInputs = array.map((_, index) => document.getElementById(`barcode-${index}-input`));
    appComponents.barcodeInputTicks = array.map((_, index) => document.getElementById(`barcode-${index}-input-tick`));
  }
  
  function resetInputs() {
    resetInputNodes();
  
    appComponents.barcodeInputs = [];
    appComponents.barcodeInputTicks = [];
  }
  
  function addInputNodes(count) {
    const array = Array(count).fill(0);
    const inputsNode = createElementFromHTML(
      `
        <div class="input-fields-list">
          ${array.map((_, index) => getInputString(index)).join('')}
        </div>
      `
    );
  
    document.querySelector('#input-fields').appendChild(inputsNode);
  }
  
  function resetInputNodes() {
    const inputs = document.querySelectorAll('.input-field');
  
    Array.from(inputs).forEach(input => input.remove());
  }
  
  function getInputString(index) {
    return `
      <div class="field input-field">
        <div class="ui corner labeled left icon input">
          <i class="barcode icon">
          </i>
          <input type="text" id="barcode-${index}-input" name="barcode-${index}" placeholder="Barcode ${index + 1}">
          <a id="barcode-${index}-input-tick" class="ui green corner label opacity-hidden fading">
            <i class="checkmark icon">
            </i>
          </a>
        </div>
      </div>
    `;
  }
  
  
  /**
   * Tabs manipulation
   */
  
  
  function addTabsContents(count) {
    const array = Array(count).fill(0);
  
    const tabsNodes = createElementFromHTML(
      `
        <div class="tabs-contents-list">
          ${array.map((_, index) => getTabsContentString(index, PICKER_SYMBOLOGIES)).join('')}
        </div>
      `
    );
  
    document.querySelector('#tabs-contents').append(tabsNodes);
  
    appComponents.symbologiesSelects = Array.from(document.querySelectorAll('.symbologies-select'))
      .map((element, index) => {
        const select = new mdc.select.MDCSelect(element);
  
        select.listen('MDCSelect:change', () => onSymbologiesChange({ index, value: select.value }));
  
        return select;
      });
  
  
    appComponents.regExpInputs = Array.from(document.querySelectorAll('.regexp-input'))
      .map((element, index) => {
        const textField = new mdc.textField.MDCTextField(element);
  
        element.querySelector('.mdc-text-field__input')
          .addEventListener('input', event => onRegExpChange({ index, value: event.target.value }));
  
        element.querySelector('.mdc-text-field__input')
          .addEventListener('blur', event => onRegExpBlur({ index, value: event.target.value }));
  
        return textField;
      });
  }
  
  function addTabs(count) {
    const currentTabsCount = document.querySelectorAll('.mdc-tab').length;
    const array = Array(count).fill(0);
  
    const tabsNodes = createElementsFromHTML(
      `${array.map((_, index) => getTabString(index + currentTabsCount)).join('')}`
    );
  
    document.querySelector('#tabs-container').append(...tabsNodes);
  }
  
  function removeTabs(count) {
    const tabs = document.querySelectorAll('.mdc-tab');
  
    Array(count).fill(0).forEach((_, index) => {
      tabs[tabs.length - 1 - index].remove();
    })
  }
  
  function removeAllTabs() {
    const tabs = document.querySelectorAll('.mdc-tab');
  
    Array.from(tabs).forEach(element => element.remove());
  }
  
  function getTabString(index) {
    return `
      <button class="mdc-tab mdc-tab--smaller-padding ${index === 0 ? 'mdc-tab--active' : ''}" role="tab" aria-selected="true" tabindex="${index}">
        <span class="mdc-tab__content">
          <span class="mdc-tab__text-label">Field ${index + 1}</span>
        </span>
        <span class="mdc-tab-indicator ${index === 0 ? 'mdc-tab-indicator--active' : ''}">
          <span class="mdc-tab-indicator__content mdc-tab-indicator__content--underline"></span>
        </span>
        <span class="mdc-tab__ripple"></span>
      </button>
    `;
  }
  
  function updateInputStyling(index) {
    const input = document.querySelector(`#regexp-field-${index}`);
  
    input.querySelector('.mdc-notched-outline').classList.add('mdc-notched-outline--upgraded')
    input.querySelector('.mdc-notched-outline').classList.add('mdc-notched-outline--notched')
    input.querySelector('.mdc-floating-label').classList.add('mdc-floating-label--float-above')
    input.querySelector('.mdc-notched-outline__notch').style.width = '50px';
  }
  
  function getTabsContentString(index, pickerSymbologies) {
    return `
      <div class="tab-content ${index === 0 ? 'tab-content--active' : ''}">
        <div class="mdc-select mdc-select--outlined symbologies-select">
          <div class="mdc-select__anchor">
            <div class="mdc-notched-outline">
              <div class="mdc-notched-outline__leading"></div>
              <div class="mdc-notched-outline__notch">
                <label class="mdc-floating-label">Symbology</label>
              </div>
              <div class="mdc-notched-outline__trailing"></div>
            </div>
  
            <i class="mdc-select__dropdown-icon"></i>
            <input disabled readonly class="mdc-select__selected-text" />
          </div>
  
          <div class="mdc-select__menu mdc-menu mdc-menu-surface">
            <ul class="mdc-list">
              ${pickerSymbologies.map((symbology, index) => {
                return `
                  <li class="mdc-list-item" ${index === 0 ? 'mdc-list-item--selected' : ''} data-value="${symbology.value}">
                    ${symbology.label}
                  </li>
                `;
              }).join('')}
            </ul>
          </div>
        </div>
  
        <div id="regexp-field-${index}" class="mdc-text-field mdc-text-field--outlined regexp-input">
          <input class="mdc-text-field__input" id="regexp-input-${index}">
          <div class="mdc-notched-outline">
            <div class="mdc-notched-outline__leading"></div>
            <div class="mdc-notched-outline__notch">
              <label for="regexp-input-${index}" class="mdc-floating-label">Regexp</label>
            </div>
            <div class="mdc-notched-outline__trailing"></div>
          </div>
        </div>
  
        <div id="regexp-error-${index}" class="mdc-text-field-helper-line  regexp-error">
          <div class="mdc-text-field-helper-text mdc-text-field-helper-text--validation-msg" role="alert">Invalid reg exp</div>
        </div>
      </div>
    `;
  }
  