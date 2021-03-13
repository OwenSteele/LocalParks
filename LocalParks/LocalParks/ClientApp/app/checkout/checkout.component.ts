import { Component } from "@angular/core";
import { ShopService } from '../data/shopService';
import { Router } from "@angular/router";

@Component({
    selector: "checkout",
    templateUrl: "checkout.component.html",
    styleUrls: []
})
export class Checkout {

    constructor(public data: ShopService,
        private router: Router) {
    }

    errorMessage: string = "";

    onCheckout() {

        if (this.data.IsSignedIn) {
            return this.data.checkout()
                .subscribe(success => {
                if (success) {
                    this.router.navigate([""]);
                }
            }, err => this.errorMessage = "Failed to checkout.");
        }
        else {
            return this.data.getToken()
                .subscribe(success => {
                    if (success) {
                        this.router.navigate([""]);
                    }
                    else {
                        this.router.navigate(["login"]);
                    }
                })
        }
    }
}