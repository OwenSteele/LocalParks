import { __decorate } from "tslib";
import { HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { map } from 'rxjs/operators';
import { Order } from "./order";
import { OrderItem } from "./orderItem";
let ShopService = class ShopService {
    constructor(http) {
        this.http = http;
        this.token = "";
        this.order = new Order();
        this.products = [];
        this.memberships = [];
    }
    get IsSignedIn() {
        return this.token.length > 0 || this.tokenExpiration <= new Date;
    }
    checkout() {
        return this.http.post("/api/shop/orders", this.order, {
            headers: new HttpHeaders().set("Authorisation", "Bearer " + this.token)
        })
            .pipe(map(response => {
            this.order = new Order;
            return true;
            // add in page redirect
        }));
    }
    getProducts() {
        return this.http.get("/api/shop/products")
            .pipe(map((data) => {
            this.products = data;
            return true;
        }));
    }
    getMemberships() {
        return this.http.get("/api/shop/products/memberships")
            .pipe(map((data) => {
            this.memberships = data;
            return true;
        }));
    }
    AddItemToOrder(product) {
        let item = this.order.items.find(i => i.productId == product.productId);
        if (item) {
            item.quantity++;
        }
        else {
            item = new OrderItem();
            item.productId = product.productId;
            item.unitPrice = product.price;
            item.productName = product.name;
            item.productCategory = product.category;
            item.quantity = 1;
            this.order.items.push(item);
        }
    }
};
ShopService = __decorate([
    Injectable()
], ShopService);
export { ShopService };
//# sourceMappingURL=shopService.js.map