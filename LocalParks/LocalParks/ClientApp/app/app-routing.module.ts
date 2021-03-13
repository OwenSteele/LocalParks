import { NgModule } from '@angular/core';
import { RouterModule, Routes, Route } from '@angular/router';

import { AppComponent } from "./app.component"
import { Shop } from "./shop/shop.component"
import { Checkout } from "./checkout/checkout.component"

const routes: Routes = [
    { path: "", component: Shop },
    { path: "checkout", component: Checkout }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, {
        useHash: true,
        enableTracing: true
    })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
