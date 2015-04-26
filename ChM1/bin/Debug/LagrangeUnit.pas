unit LagrangeUnit; //���������� ����������������� �������� ��������

interface
  uses PointsArrayUnit;
  function CalculateLagrangePolynomial(input:tpoints; k:integer; nodesInput:tpoints;
               nodesOutput:tpoints; nodesLength:integer):tpoints;
  function CalculateLagrangePolynomialForDerivative(input:tpoints; k:integer; nodesInput:tpoints;
               nodesOutput:tpoints; nodesLength:integer):tpoints;

implementation
  function CalculateLagrangePolynomial(input:tpoints; k:integer; nodesInput:tpoints;
               nodesOutput:tpoints; nodesLength:integer):tpoints;
     //k - ����������� ����� ������� ����� input, � ������� �������� �������
     //nodesInput, nodesOutput - ���� ������������ � �������� ��� � ���
     //nodesLength - ���������� �����
    var  i,j,t,n:integer; x,P,S:real;
    begin
        n:=nodesLength - 1;
        for t:=0 to (k-1) do
          begin
            x:=input[t]; //�����
            S:=0; //���������� �����
            P:=1; //���������� ������������
            for i:=0 to n  do
              begin
                P:=nodesOutput[i];
                j:=0;
                while ((j<=n) and (P<>0))  do
                  begin
                   if (j<>i) then P:=P*(x-nodesInput[j])/(nodesInput[i]-nodesInput[j]);
                   j:=j+1;
                  end;
                S:=S+P;
              end;
            result[t]:=S; //�������� �������� � �����
          end;
    end;
    
  function CalculateLagrangePolynomialForDerivative(input:tpoints; k:integer; nodesInput:tpoints;
               nodesOutput:tpoints; nodesLength:integer):tpoints;
     // ���������������� ������� �������� ��� �����������
     //k - ����������� ����� ������� ����� input, � ������� �������� �������
     //nodesInput, nodesOutput - ���� ������������ � �������� ��� � ���
     //nodesLength - ���������� �����
    var  i,j,t,m,c,n:integer; x,P,P1,P2,P3,S,S2:real;
    begin
        n:=nodesLength-1;
        for t:=0 to (k-1) do
          begin
            x:=input[t]; //�����
            S:=0; //���������� �����
            P:=1; //���������� ������������
            for i:=0 to n  do
              begin
                P:=nodesOutput[i];
                S2:=0; //���������� ���������� �����
                for j:=0 to n do
                  begin
                    if (j<>i) then
                      begin
                        P1:=1; //���������� ������������
                        for m:=0 to n do
                           if ((m<>i) and (m<>j)) then P1:=P1*(x-nodesInput[m]);
                        P2:=1; //���������� ������������
                        for c:=0 to n do
                           if (c<>i) then P2:=P2*(nodesInput[i]-nodesInput[c]);
                        S2:=S2+P1/P2;
                      end;
                  end;
                P:=P*S2;
                S:=S+P;
              end;
            result[t]:=S; //�������� �������� � �����
          end;
    end;
end.