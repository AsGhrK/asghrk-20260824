import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { authGuard } from './core/guards/auth.guard';
import { UsuarioListComponent } from './features/usuarios/usuario-list.component';
import { UsuarioFormComponent } from './features/usuarios/usuario-form.component';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    path: 'usuarios',
    canActivate: [authGuard],
    children: [
      { path: '', component: UsuarioListComponent },
      { path: 'novo', component: UsuarioFormComponent },
      { path: ':id/editar', component: UsuarioFormComponent },
    ],
  },
  { path: '', pathMatch: 'full', redirectTo: 'unidades' },
  { path: '**', redirectTo: 'unidades' },
];
