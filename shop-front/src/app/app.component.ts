import {
  AfterViewInit,
  ChangeDetectorRef,
  Component,
  OnInit,
} from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { BasecategorypriceComponent } from './components/basecategoryprice/basecategoryprice.component';
import { CategoriesListComponent } from './components/categories-list/categories-list.component';
import { ModalComponent } from './components/modal/modal.component';
import { SideBarComponent } from './components/modal/side-bar/side-bar.component';
import { ModalNavigateService } from './Services/modal-navigate.service';
import { LoginComponent } from './components/login/login.component';
import { AuthenticationService } from './Services/authentication.service';
import { LoadingComponent } from './components/loading/loading.component';

@Component({
  selector: 'app-root',
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
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  title = 'shop-front';
  isLoading = true;

  constructor(
    private modal: ModalNavigateService,
    private auth: AuthenticationService
  ) {}
  ngOnInit(): void {
    this.isLoading = false;
  }

  showSideBar() {
    this.modal.goToModal([SideBarComponent.Path]);
  }

  isAuth() {
    return this.auth.isAuthenticated();
  }
}
