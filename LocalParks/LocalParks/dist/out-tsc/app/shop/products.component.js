import { __decorate } from "tslib";
import { Component } from "@angular/core";
let Products = class Products {
    constructor(data) {
        this.data = data;
    }
    ngOnInit() {
        this.data.getProducts()
            .subscribe(success => {
            if (success) {
                this.products = this.data.products;
            }
        });
    }
    addProduct(product) {
        this.data.AddItemToOrder(product);
    }
};
Products = __decorate([
    Component({
        selector: "shop-products",
        templateUrl: "products.component.html",
        styleUrls: ["products.component.css"]
    })
], Products);
export { Products };
//# sourceMappingURL=products.component.js.map