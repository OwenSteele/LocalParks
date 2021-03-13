import { __decorate } from "tslib";
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Shop } from "./shop/shop.component";
import { Checkout } from "./checkout/checkout.component";
const routes = [
    { path: "", component: Shop },
    { path: "checkout", component: Checkout }
];
let AppRoutingModule = class AppRoutingModule {
};
AppRoutingModule = __decorate([
    NgModule({
        imports: [RouterModule.forRoot(routes, {
                useHash: true,
                enableTracing: true
            })],
        exports: [RouterModule]
    })
], AppRoutingModule);
export { AppRoutingModule };
//# sourceMappingURL=app-routing.module.js.map