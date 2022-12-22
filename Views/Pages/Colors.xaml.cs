// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using Wpf.Ui.Common.Interfaces;
using DarkMode_2.ViewModels;

namespace DarkMode_2.Views.Pages;

/// <summary>
/// Interaction logic for Colors.xaml
/// </summary>
public partial class Colors : INavigableView<ColorsViewModel>
{
    public ColorsViewModel ViewModel
    {
        get;
    }

    public Colors(ColorsViewModel viewModel)
    {
        ViewModel = viewModel;

        InitializeComponent();
    }
}
