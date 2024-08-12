import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ModalNavigateService } from '../../../Services/modal-navigate.service';
import { CategoriesListComponent } from '../../categories-list/categories-list.component';
import { AppComponent } from '../../../app.component';
import { Router, RouterModule } from '@angular/router';
import { AuthenticationService } from '../../../Services/authentication.service';
import { GroupsListComponent } from '../../groups-list/groups-list.component';
import { ProductsListComponent } from '../../products-list/products-list.component';
import { SignUpComponent } from '../../sign-up/sign-up.component';
import { MainPageComponent } from '../../main-page/main-page.component';
import { LoginComponent } from '../../login/login.component';

@Component({
  selector: 'app-side-bar',
  standalone: true,
  imports: [RouterModule],
  templateUrl: './side-bar.component.html',
  styleUrl: './side-bar.component.css',
})
export class SideBarComponent {
  static Path = 'side-bar';

  links = [
    { name: 'الصفحة الرئيسية', path: MainPageComponent.Path },
    { name: 'العيارات', path: CategoriesListComponent.Path },
    { name: 'المجموعات', path: GroupsListComponent.Path },
    { name: 'المنتجات', path: ProductsListComponent.Path },
    { name: 'تسجيل المدراء', path: [SignUpComponent.Path, 'admin'] },
    { name: 'تسجيل العاملين', path: [SignUpComponent.Path, 'worker'] },
  ];

  constructor(
    private modal: ModalNavigateService,
    private router: Router,
    private authService: AuthenticationService
  ) {
    this.name = this.authService.user?.name ?? '';
    this.role = this.authService.user?.role ?? '';
  }

  dismiss() {
    this.modal.dismiss();
  }

  name!: string;
  role!: string;

  logout() {
    this.dismiss();
    this.authService.clearToken();
    this.router.navigate([
      { outlets: { primary: [LoginComponent.Path], modal: null } },
    ]);
  }

  goto(path: string | string[]) {
    this.dismiss();
    this.router.navigate([
      { outlets: { primary: path === '' ? null : path, modal: null } },
    ]);
  }
}
