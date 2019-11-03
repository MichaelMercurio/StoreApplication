import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { ProductsComponent } from './components/products/products.component';
import { CreateComponent } from './components/create/create.component';
import { PurchasesComponent } from './components/purchases/purchases.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { AuthGuardService } from './auth/auth-guard.service';

const routes: Routes = [
  { path: '', component: LoginComponent, pathMatch: 'full' },
  { path: 'products', component: ProductsComponent, canActivate: [AuthGuardService], children: [{ path: '', outlet: 'navmenu', component: NavMenuComponent }] },
  { path: 'create', component: CreateComponent },
  { path: 'purchases', component: PurchasesComponent, canActivate: [AuthGuardService], children: [{ path: '', outlet: 'navmenu', component: NavMenuComponent }] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
