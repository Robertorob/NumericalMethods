unit TaylorUnit; //���������� ���� �������

interface
  uses PointsArrayUnit;
  function CalculateAsTaylorSeries(input:tpoints; k:integer; eps:real):tpoints;
  function CalculateAsTaylorSeriesForDerivative(input:tpoints; k:integer; eps:real):tpoints;

implementation
  function CalculateAsTaylorSeries(input:tpoints; k:integer; eps:real):tpoints; //���������� ���� �������
   // 2/sqrt(�)* �����((-1)^n * x^(2n+1)/(n!(2n+1)))
     //k - ����������� ����� �������, eps - �����������
    var  i:integer; a,x:real; n:integer;
    begin
        for i:=0 to (k-1) do
          begin
            x:=input[i]; //�����
            result[i]:=0; //�������� ���� � �����
            a:=2/sqrt(pi)*x; //������ ���� ���� (n=0)
            n:=0; //����� ����� ���� � �����
            while abs(a)>=eps do
              begin
                result[i]:=result[i]+a; //������������� ����� � �����
                //��������� ��������� ������� ����
                a:=a*(-1)*sqr(x)*(2*n+1)/(2*n+3)/(n+1);
                n:=n+1;
              end;
          end;
    end;
    
  function CalculateAsTaylorSeriesForDerivative(input:tpoints; k:integer; eps:real):tpoints;
     //���������� ���� ������� ��� �����������
        // 2/sqrt(�)* �����((-1)^n * x^2n/n!)
     //k - ����������� ����� �������, eps - �����������
     var  i:integer; a,x:real; n:integer;
     begin
         for i:=0 to (k-1) do
           begin
             x:=input[i]; //�����
             result[i]:=0; //�������� ���� � �����
             a:=2/sqrt(pi); //������ ���� ���� (n=0)
             n:=0; //����� ����� ���� � �����
             while abs(a)>=eps do
               begin
                 result[i]:=result[i]+a; //������������� ����� � �����
                 //��������� ��������� ������� ����
                 a:=a*(-1)*sqr(x)/(n+1);
                 n:=n+1;
               end;
           end;
     end;
end.