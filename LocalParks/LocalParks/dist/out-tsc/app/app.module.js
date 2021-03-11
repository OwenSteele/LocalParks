import { __decorate } from "tslib";
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Shop } from './shop/shop.component';
import { Products } from './shop/products.component';
import { Checkout } from './checkout/checkout.component';
import { Cart } from './shop/cart.component';
import { ShopService } from "./shared/shopService";
let routes = [
    { path: "", component: AppComponent },
    { path: "home", component: Shop },
    { path: "checkout", component: Checkout }
];
let AppModule = class AppModule {
};
AppModule = __decorate([
    NgModule({
        declarations: [
            AppComponent,
            Shop,
            Products,
            Cart,
            Checkout
        ],
        exports: [
            AppComponent
        ],
        imports: [
            BrowserModule,
            AppRoutingModule
        ],
        providers: [
            ShopService
        ],
        bootstrap: [AppComponent]
    })
], AppModule);
export { AppModule };
//# sourceMappingURL=app.module.js.map