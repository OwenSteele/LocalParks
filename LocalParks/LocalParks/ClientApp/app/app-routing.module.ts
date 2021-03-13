import { NgModule } from '@angular/core';
import { RouterModule, Routes, Route } from '@angular/router';

import { Shop } from "./shop/shop.component"
import { Checkout } from "./checkout/checkout.component"
import { Login } from "./login/login.component"

const routes: Routes = [
    { path: "", component: Shop },
    { path: "checkout", component: Checkout },
    { path: "login", component: Login }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, {
        useHash: true,
        enableTracing: true
    })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
