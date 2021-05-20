import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from "@angular/common/http";

import { AppRoutingModule } from './app-routing.module';

import { CompletedOrder } from "./checkout/order.component"
import { AppComponent } from './app.component';
import { Checkout } from "./checkout/checkout.component";
import { Products } from "./shop/products.component";
import { Login } from "./login/login.component";
import { Cart } from "./shop/cart.component";
import { Shop } from "./shop/shop.component";

import { ShopService } from "./data/shopService";
import { DatePipe } from '@angular/common';

@NgModule({
  declarations: [
        AppComponent,
        Shop,
        Products,
        Cart,
        Checkout,
        Login,
        CompletedOrder
    ],
    exports: [
        AppComponent
    ],
    imports: [
        HttpClientModule,
        BrowserModule,
        AppRoutingModule,
        FormsModule
  ],
    providers: [
        ShopService,
        DatePipe
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
