import { __awaiter } from "tslib";
import { browser, logging } from 'protractor';
import { AppPage } from './app.po';
describe('workspace-project App', () => {
    let page;
    beforeEach(() => {
        page = new AppPage();
    });
    it('should display welcome message', () => __awaiter(void 0, void 0, void 0, function* () {
        yield page.navigateTo();
        expect(yield page.getTitleText()).toEqual('Client-App app is running!');
    }));
    afterEach(() => __awaiter(void 0, void 0, void 0, function* () {
        // Assert that there are no errors emitted from the browser
        const logs = yield browser.manage().logs().get(logging.Type.BROWSER);
        expect(logs).not.toContain(jasmine.objectContaining({
            level: logging.Level.SEVERE,
        }));
    }));
});
//# sourceMappingURL=app.e2e-spec.js.map