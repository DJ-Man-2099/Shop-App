import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BasecategorypriceComponent } from '../basecategoryprice/basecategoryprice.component';
import { CategoriesListComponent } from '../categories-list/categories-list.component';
import { LoadingComponent } from '../loading/loading.component';
import { LoginComponent } from '../login/login.component';
import { ModalComponent } from '../modal/modal.component';
import { SideBarComponent } from '../modal/side-bar/side-bar.component';
import { AuthenticationService } from '../../Services/authentication.service';
import { ModalNavigateService } from '../../Services/modal-navigate.service';

@Component({
  selector: 'app-main-page',
  standalone: true,
  imports: [
    RouterOutlet,
    BasecategorypriceComponent,
    CategoriesListComponent,
    ModalComponent,
    SideBarComponent,
    LoginComponent,
    LoadingComponent,
  ],
  templateUrl: './main-page.component.html',
  styleUrl: './main-page.component.css',
})
export class MainPageComponent {
  static Path = '';

  constructor(
    private modal: ModalNavigateService,
    private auth: AuthenticationService
  ) {}
  showSideBar() {
    this.modal.goToModal([SideBarComponent.Path]);
  }
}
