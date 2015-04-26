unit TaylorUnit; //вычисление ряда Тейлора

interface
  uses PointsArrayUnit;
  function CalculateAsTaylorSeries(input:tpoints; k:integer; eps:real):tpoints;
  function CalculateAsTaylorSeriesForDerivative(input:tpoints; k:integer; eps:real):tpoints;

implementation
  function CalculateAsTaylorSeries(input:tpoints; k:integer; eps:real):tpoints; //вычисление ряда Тейлора
   // 2/sqrt(п)* Сумма((-1)^n * x^(2n+1)/(n!(2n+1)))
     //k - фактическая длина массива, eps - погрешность
    var  i:integer; a,x:real; n:integer;
    begin
        for i:=0 to (k-1) do
          begin
            x:=input[i]; //точка
            result[i]:=0; //значение ряда в точке
            a:=2/sqrt(pi)*x; //первый член ряда (n=0)
            n:=0; //номер члена ряда в сумме
            while abs(a)>=eps do
              begin
                result[i]:=result[i]+a; //пересчитываем сумму в точке
                //вычисляем следующий элемент ряда
                a:=a*(-1)*sqr(x)*(2*n+1)/(2*n+3)/(n+1);
                n:=n+1;
              end;
          end;
    end;
    
  function CalculateAsTaylorSeriesForDerivative(input:tpoints; k:integer; eps:real):tpoints;
     //вычисление ряда Тейлора для производной
        // 2/sqrt(п)* Сумма((-1)^n * x^2n/n!)
     //k - фактическая длина массива, eps - погрешность
     var  i:integer; a,x:real; n:integer;
     begin
         for i:=0 to (k-1) do
           begin
             x:=input[i]; //точка
             result[i]:=0; //значение ряда в точке
             a:=2/sqrt(pi); //первый член ряда (n=0)
             n:=0; //номер члена ряда в сумме
             while abs(a)>=eps do
               begin
                 result[i]:=result[i]+a; //пересчитываем сумму в точке
                 //вычисляем следующий элемент ряда
                 a:=a*(-1)*sqr(x)/(n+1);
                 n:=n+1;
               end;
           end;
     end;
end.