import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { authGuard } from './core/guards/auth.guard';
import { UsuarioListComponent } from './features/usuarios/usuario-list.component';
import { UsuarioFormComponent } from './features/usuarios/usuario-form.component';
import { UnidadeListComponent } from './features/unidades/unidade-list.component';
import { UnidadeFormComponent } from './features/unidades/unidade-form.component';
import { ColaboradorListComponent } from './features/colaboradores/colaborador-list.component';
import { ColaboradorFormComponent } from './features/colaboradores/colaborador-form.component';

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
  {
    path: 'unidades',
    canActivate: [authGuard],
    children: [
      { path: '', component: UnidadeListComponent },
      { path: 'novo', component: UnidadeFormComponent },
    ],
  },
  {
    path: 'colaboradores',
    canActivate: [authGuard],
    children: [
      { path: '', component: ColaboradorListComponent },
      { path: 'novo', component: ColaboradorFormComponent },
      { path: ':id/editar', component: ColaboradorFormComponent },
    ],
  },
  { path: '', pathMatch: 'full', redirectTo: 'unidades' },
  { path: '**', redirectTo: 'unidades' },
];
