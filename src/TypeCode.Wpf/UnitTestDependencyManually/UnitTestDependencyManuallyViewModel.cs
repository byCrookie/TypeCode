﻿using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using TypeCode.Business.Mode;
using TypeCode.Business.Mode.UnitTestDependency.Manually;
using TypeCode.Wpf.Helper.Navigation;
using TypeCode.Wpf.Helper.ViewModel;

namespace TypeCode.Wpf.UnitTestDependencyManually
{
    public class UnitTestDependencyManuallyViewModel : Reactive, IAsyncNavigatedTo
    {
        private readonly ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> _unitTestDependencyManuallyGenerator;

        public UnitTestDependencyManuallyViewModel(
            ITypeCodeGenerator<UnitTestDependencyManuallyGeneratorParameter> unitTestDependencyManuallyGenerator
        )
        {
            _unitTestDependencyManuallyGenerator = unitTestDependencyManuallyGenerator;
        }
        
        public Task OnNavigatedToAsync(NavigationContext context)
        {
            GenerateCommand = new AsyncCommand(GenerateAsync);
            return Task.CompletedTask;
        }

        private async Task GenerateAsync()
        {
            var parameter = new UnitTestDependencyManuallyGeneratorParameter
            {
                Input = Input
            };
            
            var result = await _unitTestDependencyManuallyGenerator.GenerateAsync(parameter);
            Output = result;
        }
        
        public ICommand GenerateCommand { get; set; }
        
        public string Input {
            get => Get<string>();
            set => Set(value);
        }

        public string Output {
            get => Get<string>();
            private set => Set(value);
        }
    }
}