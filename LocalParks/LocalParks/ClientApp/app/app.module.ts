import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from "@angular/common/http";

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { Shop } from "./shop/shop.component"
import { Products } from "./shop/products.component"
import { Cart } from "./shop/cart.component"

import { ShopService } from "./data/shopService"

@NgModule({
  declarations: [
        AppComponent,
        Shop,
        Products,
        Cart
    ],
    exports: [
        AppComponent
    ],
    imports: [
        HttpClientModule,
        BrowserModule,
      AppRoutingModule      
  ],
    providers: [
        ShopService
    ],
  bootstrap: [AppComponent]
})
export class AppModule { }
