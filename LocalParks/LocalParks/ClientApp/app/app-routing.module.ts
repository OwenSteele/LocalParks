import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { Shop } from "./shop/shop.component"
import { Checkout } from "./checkout/checkout.component"
import { Login } from "./login/login.component"
import { CompletedOrder } from "./checkout/order.component"

const routes: Routes = [
    { path: "", component: Shop },
    { path: "checkout", component: Checkout },
    { path: "login", component: Login },
    { path: "order", component: CompletedOrder }
];

@NgModule({
    imports: [RouterModule.forRoot(routes, {
        useHash: true,
        enableTracing: true
    })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
