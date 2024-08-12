import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-sign-up',
  standalone: true,
  imports: [],
  templateUrl: './sign-up.component.html',
  styleUrl: './sign-up.component.css',
})
export class SignUpComponent implements OnInit {
  static Path = 'signUp';

  type!: string;

  constructor(private currentRoute: ActivatedRoute) {}

  ngOnInit(): void {
    this.type = this.currentRoute.snapshot.params['role'];
    console.log(this.type);
  }
}
