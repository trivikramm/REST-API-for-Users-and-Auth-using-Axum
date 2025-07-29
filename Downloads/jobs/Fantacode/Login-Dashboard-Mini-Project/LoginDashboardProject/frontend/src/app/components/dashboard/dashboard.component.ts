import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  
  // Hardcoded data for the chart
  chartData = [
    { name: 'Open', value: 20 },
    { name: 'In Progress', value: 50 },
    { name: 'Closed', value: 100 }
  ];

  // Chart options
  view: [number, number] = [700, 400];
  showXAxis = true;
  showYAxis = true;
  gradient = false;
  showLegend = true;
  showXAxisLabel = true;
  xAxisLabel = 'Status';
  showYAxisLabel = true;
  yAxisLabel = 'Ticket Count';
  colorScheme = 'vivid';
  
  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit(): void {
    // Lifecycle hook, can be used for initialization logic
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}