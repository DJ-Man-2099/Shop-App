import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BasecategorypriceComponent } from './components/basecategoryprice/basecategoryprice.component';
import { CategoriesListComponent } from './components/categories-list/categories-list.component';
import { ModalComponent } from './components/modal/modal.component';
import { SideBarComponent } from './components/modal/side-bar/side-bar.component';
import { ModalNavigateService } from './Services/modal-navigate.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    BasecategorypriceComponent,
    CategoriesListComponent,
    ModalComponent,
    SideBarComponent,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'shop-front';

  constructor(private modal: ModalNavigateService) {}

  showSideBar() {
    this.modal.goToModal([SideBarComponent.Path]);
  }
}
