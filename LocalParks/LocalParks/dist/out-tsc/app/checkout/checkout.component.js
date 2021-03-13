import { __decorate } from "tslib";
import { Component } from "@angular/core";
let Checkout = class Checkout {
    constructor(data, router) {
        this.data = data;
        this.router = router;
        this.errorMessage = "";
    }
    onCheckout() {
        return this.data.checkout().subscribe(success => {
            if (success) {
                this.router.navigate(["/"]);
            }
        }, err => this.errorMessage = "Failed to checkout.");
    }
};
Checkout = __decorate([
    Component({
        selector: "checkout",
        templateUrl: "checkout.component.html",
        styleUrls: []
    })
], Checkout);
export { Checkout };
//# sourceMappingURL=checkout.component.js.map