import { Component } from "@angular/core";
import { ShopService } from "../data/shopService";
import { Router } from "@angular/router";

@Component({
    selector: "user-cart",
    templateUrl: "cart.component.html",
    styleUrls: ["cart.component.css"]
})
export class Cart {
    constructor(public data: ShopService,
        private router: Router) { }

    onCheckout() {
        if (this.data.SignInRequired) {
            this.data.getToken()
            .subscribe(success => {
                if (success) {
                    this.router.navigate(["checkout"]);
                }
                else {
                    this.router.navigate(["login"]);
                }
            })
        }
        else {
            this.router.navigate([""]);            
        }
    }
}