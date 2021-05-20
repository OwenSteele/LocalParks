import { HostListener, Component, OnInit } from "@angular/core";
import { ShopService } from "../data/shopService";


@Component({
    selector: "shop-home",
    templateUrl: "shop.component.html",
    styleUrls: ["shop.component.css"]
})
export class Shop implements OnInit{
    constructor(public data: ShopService) { }
        
    title = "The Local Parks Authority Shop";
    cartVisible = false;
    marginStyle = 'margin-left: 0px; transition: .5s;';
    sidebarStyle = 'transition: .5s;';
    cartButtonStyle = 'left: 0;'
    margin: number;

    public screenWidth: any;
    ngOnInit() {
        this.screenWidth = window.innerWidth;
    }
    @HostListener('window:resize', ['$event'])
    onResize(event: any) {
        this.screenWidth = window.innerWidth;
    }

    toggleNav() {
        this.cartVisible = !this.cartVisible
        this.setMargin()
    }
    closeNav(cartClose: any) {
        console.log("closed parent");
        this.cartVisible = false
        this.setMargin()
    }
    private setMargin() {
        if (this.cartVisible) {
            if (this.screenWidth > 500) {
                this.marginStyle = 'margin-left: 400px; transition: .5s;';
                this.cartButtonStyle = 'left: 400px;'
            }
            else {
                this.marginStyle = `margin-left: ${this.screenWidth + 100}px; transition: .5s;`;
                this.cartButtonStyle = `left: ${this.screenWidth + 100}px;`
            }
        }
        else {
            this.marginStyle = 'margin-left: 0px; transition: .5s;';
            this.cartButtonStyle = 'left: 0;'

        }
    }
}