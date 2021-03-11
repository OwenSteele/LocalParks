import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Shop } from "./shop/shop.component"
import { Checkout } from "./checkout/checkout.component"

const routes: Routes = [];

@NgModule({
    imports: [RouterModule.forRoot(routes, {
        useHash: true,
        enableTracing: true
    })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
