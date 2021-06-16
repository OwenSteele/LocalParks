import { Component } from "@angular/core";
import { ShopService } from '../data/shopService';
import { Router } from "@angular/router";

@Component({
    selector: "checkout",
    templateUrl: "checkout.component.html",
    styleUrls: ["checkout.component.css"]
})
export class Checkout {

    constructor(public data: ShopService,
        private router: Router) {
    }

    errorMessage: string = "";

    onCheckout() {

        if (this.data.SignInRequired) {
            this.data.getToken()
                .subscribe(success => {
                    if (success == false) {
                        this.router.navigate(["login"]);
                    }
                })
        }


        return this.data.checkout()
            .subscribe(success => {
            if (success) {
                this.router.navigate(["order"]);
            }
        }, err => this.errorMessage = "Failed to checkout.");
    }
}